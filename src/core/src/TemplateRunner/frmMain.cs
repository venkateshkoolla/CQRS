using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;
using Newtonsoft.Json;
using Ocas.Domestic.Apply.Enums;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Models.Templates;
using Ocas.Domestic.Apply.TemplateProcessors;

namespace TemplateRunner
{
    public partial class frmMain : Form
    {
        private readonly ITemplateProcessorSettings _templateProcessorSettings;

        public frmMain()
        {
            InitializeComponent();

            var appSettingsFetcher = new Ocas.Common.AppSettingsFetcher();
            _templateProcessorSettings = new TemplateProcessorSettings(
                appSettingsFetcher.GetSettings<string>("ocas:evopdf:licenseKey"),
                appSettingsFetcher.GetSettings<string>("ocas:evopdf:serviceUrl"),
                appSettingsFetcher.GetSettings<string>("ocas:evopdf:servicePassword"));
        }

        private static string Read(string resourceName, Assembly assembly = null)
        {
            if (assembly == null)
            {
                assembly = Assembly.GetExecutingAssembly();
            }

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // Load settings
            if (cmbTemplate.Items.Cast<string>().Any(x => x == Properties.Settings.Default.DocumentType))
            {
                cmbTemplate.SelectedItem = Properties.Settings.Default.DocumentType;
            }
            else
            {
                cmbTemplate.SelectedIndex = 0;
            }

            if (cmbTestType.Items.Cast<string>().Any(x => x == Properties.Settings.Default.TestType))
            {
                cmbTestType.SelectedItem = Properties.Settings.Default.TestType;
            }
            else
            {
                cmbTestType.SelectedIndex = 0;
            }

            switch (Properties.Settings.Default.Locale)
            {
                case "fr-CA":
                    radFrCa.Checked = true;
                    break;
                case "en-CA":
                    radEnCa.Checked = true;
                    break;
            }

            switch (Properties.Settings.Default.SchemaVersion)
            {
                case (int)PostSecondaryTranscriptVersion.X12:
                    radX12.Checked = true;
                    break;
                case (int)PostSecondaryTranscriptVersion.PESC:
                    radPesc.Checked = true;
                    break;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            var locale = radFrCa.Checked ? "fr-CA" : "en-CA";
            var version = radX12.Checked ? PostSecondaryTranscriptVersion.X12 : PostSecondaryTranscriptVersion.PESC;

            // Save settings
            Properties.Settings.Default.DocumentType = cmbTemplate.SelectedItem.ToString();
            Properties.Settings.Default.Locale = locale;
            Properties.Settings.Default.SchemaVersion = (int)version;
            Properties.Settings.Default.TestType = cmbTestType.SelectedItem.ToString();
            Properties.Settings.Default.Save();

            btnGenerate.Enabled = false;

            try
            {
                switch (cmbTemplate.SelectedItem.ToString())
                {
                    case "Post-Secondary Transcript":
                        GeneratePostSecondaryTranscript(locale, version);
                        break;
                    case "High School Grades":
                        GenerateHighSchoolGrades(locale);
                        break;
                    case "Standardized Test":
                        GenerateStandardizedTest(locale, cmbTestType.SelectedItem.ToString());
                        break;
                }
            }
            catch (Exception exc)
            {
                var message = exc.Message + Environment.NewLine + exc.StackTrace;

                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnGenerate.Enabled = true;
            }
        }

        private void GenerateHighSchoolGrades(string locale)
        {
            var json = Read("TemplateRunner.Resources.HighSchoolGrades.json");
            var model = JsonConvert.DeserializeObject<HighSchoolGradesViewModel>(json);

            var lang = locale.Replace("-", string.Empty); // msbuild does some weird things when embedded resource path contains 'en-CA' or 'fr-CA'
            var translationJson = Read($"TemplateRunner.Resources.Translations.api.{lang}.json");
            var translations = JsonConvert.DeserializeObject<Dictionary<string, string>>(translationJson);
            var translationDictionary = new TranslationsDictionary(translations);
            model.LoadTranslations(translationDictionary);

            var razorTemplateService = new RazorTemplateService(_templateProcessorSettings);
            var letter = razorTemplateService.GenerateHighSchoolGrades(model);

            SavePdf(letter);
        }

        private void GeneratePostSecondaryTranscript(string locale, PostSecondaryTranscriptVersion version)
        {
            var lang = locale.Replace("-", string.Empty); // msbuild does some weird things when embedded resource path contains 'en-CA' or 'fr-CA'
            var translationJson = Read($"TemplateRunner.Resources.Translations.api.{lang}.json");
            var translations = JsonConvert.DeserializeObject<Dictionary<string, string>>(translationJson);
            var translationDictionary = new TranslationsDictionary(translations);

            var model = new PostSecondaryTranscriptViewModel();
            switch (version)
            {
                case PostSecondaryTranscriptVersion.PESC:
                    {
                        var xml = Read("TemplateRunner.Resources.PESC.xml");
                        var doc = XDocument.Parse(xml);
                        model.LoadTranslations(translationDictionary, "Georgian");
                        model.LoadXml(doc, System.Globalization.CultureInfo.GetCultureInfo(locale), version);
                        break;
                    }

                case PostSecondaryTranscriptVersion.X12:
                    {
                        var xml = Read("TemplateRunner.Resources.X12.xml");
                        var doc = XDocument.Parse(xml);
                        model.LoadTranslations(translationDictionary, "Georgian");
                        model.LoadXml(doc, System.Globalization.CultureInfo.GetCultureInfo(locale), version);
                        break;
                    }
            }

            var razorTemplateService = new RazorTemplateService(_templateProcessorSettings);
            var letter = razorTemplateService.GeneratePostSecondaryTranscript(model);

            SavePdf(letter);
        }

        private void GenerateStandardizedTest(string locale, string testType)
        {
            var json = Read($"TemplateRunner.Resources.StandardizedTest_{testType}.json");
            var model = JsonConvert.DeserializeObject<StandardizedTestViewModel>(json);

            var lang = locale.Replace("-", string.Empty); // msbuild does some weird things when embedded resource path contains 'en-CA' or 'fr-CA'
            var translationJson = Read($"TemplateRunner.Resources.Translations.api.{lang}.json");
            var translations = JsonConvert.DeserializeObject<Dictionary<string, string>>(translationJson);
            var translationDictionary = new TranslationsDictionary(translations);
            model.LoadTranslations(translationDictionary);

            var razorTemplateService = new RazorTemplateService(_templateProcessorSettings);
            var letter = razorTemplateService.GenerateStandardizedTest(model);

            SavePdf(letter);
        }

        private void SavePdf(byte[] letter)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "PDF|*.pdf";

                var result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    using (var stream = dialog.OpenFile())
                    {
                        stream.Write(letter, 0, letter.Length);
                    }

                    // open saved file in default application
                    System.Diagnostics.Process.Start(dialog.FileName);
                }
            }
        }

        private void CmbTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            grpVersion.Visible = cmbTemplate.SelectedItem as string == "Post-Secondary Transcript";
            grpTestType.Visible = cmbTemplate.SelectedItem as string == "Standardized Test";
        }
    }
}

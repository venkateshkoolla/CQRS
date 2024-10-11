using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using EvoPdf.HtmlToPdfClient;
using Ocas.Domestic.Apply.Models.Templates;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace Ocas.Domestic.Apply.TemplateProcessors
{
    public class RazorTemplateService : IRazorTemplateService
    {
        private const string PostSecondaryTemplateKey = "PostSecondaryTemplate";
        private const string HighSchoolGradesTemplateKey = "HighSchoolGradesTemplate";
        private const string StandardizedTestTemplateKey = "StandardizedTestTemplate";
        private const string TranscriptFooterTemplateKey = "TranscriptFooterTemplate";

        private readonly IRazorEngineService _service;
        private readonly ITemplateProcessorSettings _appSettings;

        public RazorTemplateService(ITemplateProcessorSettings appSettings)
        {
            var config = new TemplateServiceConfiguration();

#if DEBUG
            config.Debug = true;
#endif

            _service = RazorEngineService.Create(config);
            _appSettings = appSettings;

            _service.Compile(Read("Ocas.Domestic.Apply.TemplateProcessors.Resources.PostSecondaryTemplate.cshtml"), PostSecondaryTemplateKey);
            _service.Compile(Read("Ocas.Domestic.Apply.TemplateProcessors.Resources.HighSchoolGradesTemplate.cshtml"), HighSchoolGradesTemplateKey);
            _service.Compile(Read("Ocas.Domestic.Apply.TemplateProcessors.Resources.StandardizedTestTemplate.cshtml"), StandardizedTestTemplateKey);
            _service.Compile(Read("Ocas.Domestic.Apply.TemplateProcessors.Resources.TranscriptFooterTemplate.cshtml"), TranscriptFooterTemplateKey);
        }

        public Task<byte[]> GeneratePostSecondaryTranscriptAsync(PostSecondaryTranscriptViewModel model)
        {
            var task = Task.Run(() => GeneratePostSecondaryTranscript(model));
            task.ConfigureAwait(false);

            return task;
        }

        public Task<byte[]> GenerateHighSchoolGradesAsync(HighSchoolGradesViewModel model)
        {
            var task = Task.Run(() => GenerateHighSchoolGrades(model));
            task.ConfigureAwait(false);

            return task;
        }

        public Task<byte[]> GenerateTestPdfAsync()
        {
            var task = Task.Run(GenerateTestPdf);
            task.ConfigureAwait(false);

            return task;
        }

        public byte[] GeneratePostSecondaryTranscript(PostSecondaryTranscriptViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var html = _service.Run(PostSecondaryTemplateKey, typeof(PostSecondaryTranscriptViewModel), model);
            var htmlFooter = _service.Run(TranscriptFooterTemplateKey, typeof(TranscriptFooterViewModel), model.FooterViewModel);

            // convert html to pdf
            var pdfConverter = GetRemotePdfConverter(htmlFooter);

            // return pdf byte array
            return pdfConverter.ConvertHtml(html, null);
        }

        public byte[] GenerateHighSchoolGrades(HighSchoolGradesViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var html = _service.Run(HighSchoolGradesTemplateKey, typeof(HighSchoolGradesViewModel), model);
            var htmlFooter = _service.Run(TranscriptFooterTemplateKey, typeof(TranscriptFooterViewModel), model.FooterViewModel);

            // convert html to pdf
            var pdfConverter = GetRemotePdfConverter(htmlFooter);

            // return pdf byte array
            return pdfConverter.ConvertHtml(html, null);
        }

        public Task<byte[]> GenerateStandardizedTestAsync(StandardizedTestViewModel model)
        {
            var task = Task.Run(() => GenerateStandardizedTest(model));
            task.ConfigureAwait(false);

            return task;
        }

        public byte[] GenerateStandardizedTest(StandardizedTestViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var html = _service.Run(StandardizedTestTemplateKey, typeof(StandardizedTestViewModel), model);

            // convert html to pdf
            var pdfConverter = GetRemotePdfConverter();

            // return pdf byte array
            return pdfConverter.ConvertHtml(html, null);
        }

        public byte[] GenerateTestPdf()
        {
            var html = _service.RunCompile("<html><body>Hello @Model.Name, templating is working!</body></html>", "PdfTest", null, new { Name = "World" });
            var pdfConverter = GetRemotePdfConverter();

            // return pdf byte array
            return pdfConverter.ConvertHtml(html, null);
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

        private HtmlToPdfConverter GetRemotePdfConverter()
        {
            var pdfConverter = new HtmlToPdfConverter
            {
                ConversionDelay = 0 //need this setting as EvoPdf adds few seconds delay to process html.
            };

            if (!string.IsNullOrWhiteSpace(_appSettings.OcasEvoPdfLicenceKey))
                pdfConverter.LicenseKey = _appSettings.OcasEvoPdfLicenceKey;

            pdfConverter.UseWebService = true;
            pdfConverter.WebServiceUrl = _appSettings.OcasEvoPdfServiceUrl;
            pdfConverter.ServicePassword = _appSettings.OcasEvoPdfServicePassword;

            // set default page size and margins
            pdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.Letter;

            // 1-inch margins
            pdfConverter.PdfDocumentOptions.TopMargin = 72;
            pdfConverter.PdfDocumentOptions.BottomMargin = 72;
            pdfConverter.PdfDocumentOptions.RightMargin = 72;
            pdfConverter.PdfDocumentOptions.LeftMargin = 72;

            return pdfConverter;
        }

        private HtmlToPdfConverter GetRemotePdfConverter(string htmlFooterTemplate)
        {
            var pdfConverter = GetRemotePdfConverter();
            if (string.IsNullOrEmpty(htmlFooterTemplate)) return pdfConverter;

            pdfConverter.PdfDocumentOptions.ShowFooter = true;
            pdfConverter.PdfDocumentOptions.BottomMargin = 0;
            pdfConverter.PdfFooterOptions.FooterHeight = 72;

            var footerWidth = pdfConverter.PdfDocumentOptions.PdfPageSize.Width
                    - pdfConverter.PdfDocumentOptions.LeftMargin - pdfConverter.PdfDocumentOptions.RightMargin;
            var footerLine = new LineElement(0, 0, footerWidth, 0)
            {
                ForeColor = RgbColor.LightGray
            };
            pdfConverter.PdfFooterOptions.AddElement(footerLine);

            var footerHtml = new HtmlToPdfVariableElement(htmlFooterTemplate, null)
            {
                FitHeight = true
            };
            pdfConverter.PdfFooterOptions.AddElement(footerHtml);

            return pdfConverter;
        }
    }
}

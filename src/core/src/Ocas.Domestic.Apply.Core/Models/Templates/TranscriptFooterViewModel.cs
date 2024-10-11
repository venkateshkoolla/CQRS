using System;

namespace Ocas.Domestic.Apply.Models.Templates
{
    public class TranscriptFooterViewModel
    {
        private readonly TranslationsDictionary _translationsDictionary;

        public TranscriptFooterViewModel(TranslationsDictionary translationsDictionary)
        {
            _translationsDictionary = translationsDictionary;
            LoadTranslations();
        }

        public DateTime CreationDate => DateTime.UtcNow;
        public TranscriptFooterLabels Labels { get; set; }

        public class TranscriptFooterLabels
        {
            public string GenerationMessage { get; set; }
            public string GenerationDateFormat { get; set; }
            public string Disclaimer { get; set; }
            public string PageNumberTitle { get; set; }
            public string PageNumberDivider { get; set; }
        }

        public void LoadTranslations()
        {
            Labels = new TranscriptFooterLabels
            {
                GenerationMessage = _translationsDictionary.Get("transcript.footer.generation_message"),
                GenerationDateFormat = _translationsDictionary.Get("transcript.footer.date_format"),
                Disclaimer = _translationsDictionary.Get("transcript.footer.disclaimer"),
                PageNumberTitle = _translationsDictionary.Get("transcript.footer.page_number_title"),
                PageNumberDivider = _translationsDictionary.Get("transcript.footer.page_number_divider")
            };
        }
    }
}

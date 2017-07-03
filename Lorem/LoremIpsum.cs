using Helper.Extensions;
using System;
using System.Net;

namespace Helper.Lorem
{
    public class LoremIpsum
    {
        private const string SERVICE_URI = "http://loripsum.net/api";

        public int ParagraphCount { get; set; } = 5;

        public ParagraphLength ParagraphLength { get; set; } = ParagraphLength.Medium;

        public bool AddHeadings { get; set; }
        public bool DecorateText { get; set; }
        public bool AddLinks { get; set; }
        public bool AddUnorderedLists { get; set; }
        public bool AddOrderedLists { get; set; }
        public bool AddDescriptionLists { get; set; }
        public bool AddBlockQuotes { get; set; }
        public bool AddCodeSamples { get; set; }
        public bool UseAllCaps { get; set; }

        public string Html => DownloadText(GetUri());

        public string PlainText => DownloadText(GetUri(true));

        public override string ToString()
        {
            return PlainText;
        }

        private string GetUri(bool isPlainText = false, bool usePrude = true) //prude == remove words like sex and homo -- WE'RE AMERICAN!
        {
            try
            {
                var paragraphLength = ParagraphLength.GetAttributeOfType<ServiceParameterAttribute>().Parameter;

                var uri =
                    $"{SERVICE_URI}/{ParagraphCount}{(string.IsNullOrEmpty(paragraphLength) ? string.Empty : "/" + paragraphLength)}";

                if (AddHeadings)
                    uri += "/headers";

                if (DecorateText)
                    uri += "/decorate";

                if (AddLinks)
                    uri += "/link";

                if (AddUnorderedLists)
                    uri += "/ul";

                if (AddOrderedLists)
                    uri += "/ol";

                if (AddDescriptionLists)
                    uri += "/dl";

                if (AddBlockQuotes)
                    uri += "/bq";

                if (AddCodeSamples)
                    uri += "/code";

                if (UseAllCaps)
                    uri += "/allcaps";

                if (isPlainText)
                    uri += "/plaintext";

                if (usePrude)
                    uri += "/prude";

                return uri.ToLower();
            }
            catch (Exception ex)
            {
                throw new Exception("error on GetUri method, message: " + ex.Message);
            }
        }

        private string DownloadText(string sourceUri)
        {
            try
            {
                using (var client = new WebClient())
                {
                    return client.DownloadString(sourceUri);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error on DownloadText method, message: " + ex.Message);
            }
        }
    }
}

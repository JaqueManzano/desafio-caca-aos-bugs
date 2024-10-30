namespace Dima.Core.Responses
{
    public class ErrorResponse
    {
        public string? Type { get; set; }
        public string? Title { get; set; }
        public int Status { get; set; }
        public Dictionary<string, string[]> Errors { get; set; }

        public List<string> GetErrorMessages()
        {
            var messages = new List<string>();

            if (Errors != null)
            {
                foreach (var error in Errors)
                {
                    messages.AddRange(error.Value);
                }
            }

            return messages;
        }
    }

}

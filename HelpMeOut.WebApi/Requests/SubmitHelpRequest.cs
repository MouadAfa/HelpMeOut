using HelpMeOut.Models;

namespace HelpMeOut.WebApi.Requests
{
    public class SubmitHelpRequest
    {
        public string SeekerEmail { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public RequestType Type { get; set; }
        public List<Rule> Rules { get; set; }
    }
}
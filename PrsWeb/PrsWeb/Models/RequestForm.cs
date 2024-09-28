namespace PrsWeb.Models
{   
    
    /*
     *  A class to hold the partial request fields
     *  which are input by the user.The remaining fields
     *  are assigned by the back end and so are not passed in the request for a request Post*/

    public class RequestForm
    {
        public int UserId { get; set; }

        public string Description { get; set; }

        public string Justification { get; set; }

        public DateOnly DateNeeded { get; set; }

        public string DeliveryMode {  get; set; }

       
    }
}

namespace Store.State
{
    public abstract class RootState
    {

        public RootState(bool isLoading, string? currentErrorMessage, string url) =>

            (IsLoading, CurrentErrorMessage, Url) = (isLoading, currentErrorMessage, url);

        public bool IsLoading { get; set; }


        public string? CurrentErrorMessage { get; set; }



        public string? Url { get; set; }


        public bool HasCurrentErrors => !string.IsNullOrWhiteSpace(CurrentErrorMessage);
    }
}

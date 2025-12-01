namespace Store.Features.Todos.Actions.LoadTodos
{
    public class LoadTodosSuccessAction
    {
        public string Url { get; set; }
        public LoadTodosSuccessAction(dynamic todos, string url) =>
            (Todos, Url) = (todos, url);


        //public LoadTodosSuccessAction(dynamic Data, string Url)
        //{
        //    this.Todos = Data;
        //    this.Url = Url;

        //}

        public LoadTodosSuccessAction(dynamic Data, object Pagination, string url)
        {
            this.Todos = Data;
            this.Pagination = Pagination;
            this.Url = url;

        }

        public dynamic Todos { get; }



        public object Pagination { get; set; }
    }
}

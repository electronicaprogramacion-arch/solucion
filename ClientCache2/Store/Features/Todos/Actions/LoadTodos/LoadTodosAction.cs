


using Helpers.Controls.ValueObjects;

namespace Store.Features.Todos.Actions.LoadTodos
{
    public class LoadTodosAction
    {

        public object Method { get; set; }
        public object Parameter { get; set; }

        public dynamic Data1 { get; set; }

        public string Url { get; set; }


        public LoadTodosAction(dynamic Data, string Url)
        {
            this.Data1 = Data;
            this.Url = Url;

        }

        public LoadTodosAction(dynamic Data, object Pagination)
        {
            this.Data1 = Data;
            this.Pagination = Pagination;

        }

        //public LoadTodosAction(object obj1,object obj2)
        //{
        //    this.Method = obj1;
        //    this.Parameter = obj2;
        //}

        public void LoadData<T>(ResultSet<T> Data)
        {
            this.Data1 = Data;
        }

        public object Pagination { get; set; }

    }
}

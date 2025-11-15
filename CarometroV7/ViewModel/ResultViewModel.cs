namespace CarometroV7.ViewModel
{
    public class ResultViewModel<T>
    {
        public ResultViewModel(T data, List<string> errors)
        {
            Data = data;
            Errors = errors;
        }

        public ResultViewModel(T data)
        {
            Data = data;
        }
        public ResultViewModel(List<string> errors)
        {
            Errors = errors;
        }

        public ResultViewModel(string error)
        {
            Errors.Add(error);
        }


        public T Data { get; private set; }
        //vale a pena aqui no C#10 que não precisamos inicaliar a lista no construtor
        public List<string> Errors { get; private set; } = new();
    }
}

namespace WebAPI.Controllers
{
    internal interface IOurcontroller<T>
    {
        // Make sure we have url generator in all controllers
        public string GetUrl(T obj);
    }
}
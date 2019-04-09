namespace com.TeamPlug.Patterns
{
    interface Builder<T> where T : class
    {
        T Build();
    }
}

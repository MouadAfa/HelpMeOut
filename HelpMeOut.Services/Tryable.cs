using LanguageExt;

namespace HelpMeOut.Services;

public class Tryable<T>
{
    public static Option<T> Try(Func<T, Option<T>> tryFunc, T item, Action<Exception> onFailure, Action<T> onAll = null)
    {
        try
        {
            return tryFunc?.Invoke(item) ?? Option<T>.None;
        }
        catch (Exception e)
        {
            onFailure?.Invoke(e);
            return Option<T>.None;
        }
        finally
        {
            onAll?.Invoke(item);
        }
    }
    
    public static Either<Error, R> Try<R>(Func<T, Either<Error, R>> tryFunc, T item, Action<Exception> onFailure, Action<T> onAll = null)
    {
        try
        {
            return tryFunc.Invoke(item);
        }
        catch (Exception e)
        {
            onFailure?.Invoke(e);
            
            return Either<Error, R>.Left(new Error
                { Message = e.Message, Exception = e });
        }
        finally
        {
            onAll?.Invoke(item);
        }
    }
}
namespace Constraintor.Core.Common;

public interface IDeepCloneable<out T>
{
    T? DeepClone();
}
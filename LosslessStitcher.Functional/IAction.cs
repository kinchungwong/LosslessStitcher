namespace LosslessStitcher.Functional
{
    public interface IAction
    {
        void Invoke();
    }

    public interface IAction<in T1>
    {
        void Invoke(T1 t1);
    }

    public interface IAction<in T1, in T2>
    {
        void Invoke(T1 t1, T2 t2);
    }

    public interface IAction<in T1, in T2, in T3>
    {
        void Invoke(T1 t1, T2 t2, T3 t3);
    }

    public interface IAction<in T1, in T2, in T3, in T4>
    {
        void Invoke(T1 t1, T2 t2, T3 t3, T4 t4);
    }

    public interface IAction<in T1, in T2, in T3, in T4, in T5>
    {
        void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5);
    }

    public interface IAction<in T1, in T2, in T3, in T4, in T5, in T6>
    {
        void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6);
    }
}

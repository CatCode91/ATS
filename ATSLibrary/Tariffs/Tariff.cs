namespace ATSLibrary.Tariffs
{
    public abstract class Tariff
    {
        public abstract string Name { get; }
        public abstract decimal Rate { get; }
    }
}

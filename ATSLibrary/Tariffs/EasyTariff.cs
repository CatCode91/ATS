namespace ATSLibrary.Tariffs
{
    public class EasyTariff : Tariff
    {
        public override string Name => @"""Легкий""";
        public override decimal Rate => 0.01M;
    }
}

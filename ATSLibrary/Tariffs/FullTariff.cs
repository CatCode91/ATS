namespace ATSLibrary.Tariffs
{
    public class FullTariff : Tariff
    {
        public override string Name => @"""Бизнес""";
        public override decimal Rate => 0.1M;
    }
}

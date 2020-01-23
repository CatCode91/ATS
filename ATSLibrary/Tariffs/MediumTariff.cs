namespace ATSLibrary.Tariffs
{
    public class MediumTariff : Tariff
    {
        public override string Name => @"""Народный""";
        public override decimal Rate => 0.05m;
    }
}

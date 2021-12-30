namespace Vierop1Rij.Domein
{
    public enum SpelStatus
    {
        /// <summary>
        /// Niet gezet. Default.
        /// </summary>
        NogNietBegonnen = 0,

        /// <summary>
        /// Rood
        /// </summary>
        Actief = 1,

        /// <summary>
        /// Rood
        /// </summary>
        Beeindigd4Op1Rij = 2,

        /// <summary>
        /// Geel
        /// </summary>
        BeeindigdBordVol = 3,

        /// <summary>
        /// Geel
        /// </summary>
        BeeindigdDoorSpeler = 3,
    }
}

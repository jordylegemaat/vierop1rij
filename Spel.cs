namespace Vierop1Rij.Domein
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Instance of a spel object.
    /// </summary>
    public class Spel
    {

        private SpelStatus status = SpelStatus.NogNietBegonnen;

        public Spel(Speler speler1, Speler speler2)
        {
            if (speler1 == null)
            {
                throw new ArgumentNullException(nameof(speler1));
            }

            if (speler2 == null)
            {
                throw new ArgumentNullException(nameof(speler2));
            }

            if (speler1.Naam == speler2.Naam)
            {
                throw new ArgumentException("Speler 2 kan niet dezelfde naam hebben als speler 1", nameof(speler2));
            }

            if (speler1.Kleur == speler2.Kleur)
            {
                throw new ArgumentException("Speler 2 kan niet dezelfde kleur hebben als speler 1", nameof(speler2));
            }

            this.Speler1 = speler1;
            this.Speler2 = speler2;

            this.Status = SpelStatus.NogNietBegonnen;
        }

        public event EventHandler<Speler> SpelStarted;

        public event EventHandler<SpelStatus> StatusChanged;

        public event EventHandler<Speler> BeurtOver;

        public Speler Speler1 { get; private set; }

        public Speler Speler2 { get; private set; }

        public Speler HuidigeSpeler { get; private set; }

        public Bord Bord { get; private set; }

        public SpelStatus Status
        {
            get
            {
                return this.status;
            }

            private set
            {
                if (value != this.status)
                {
                    this.status = value;
                    this.StatusChanged?.Invoke(this, this.Status);
                }
            }
        }

        /// <summary>
        /// Kiest een willekeurige speler uit uit de lijst.
        /// </summary>
        /// <returns>Geeft de eerste beurt aan de speler die returned uit deze functie.</returns>
        private Speler BepaalStartSpeler()
        {
            Random random = new();
            if (random.Next(0, 1) == 0)
            {
                return this.Speler1;
            }

            return this.Speler2;
        }

        private Speler GetVolgendeSpeler(Speler speler)
        {
            return speler == this.Speler1 ? this.Speler2 : this.Speler1;
        }

        public void StartSpel()
        {
            // wat moet de state van het spel worden?
            // leeg bord hebben
            this.Bord = new();

            // startspeler moet bepaalt zijn.
            this.HuidigeSpeler = this.BepaalStartSpeler();

            this.Status = SpelStatus.Actief;

            // gooi een event dat het spel gestart is
            this.SpelStarted?.Invoke(this, this.HuidigeSpeler);
        }

        public void DoeZet(Speler spelerDieDeZetDoet, int kolom)
        {
            // mag de speler wel een zet doen?
            if (spelerDieDeZetDoet != this.HuidigeSpeler)
            {
                throw new InvalidProgramException("Deze speler is niet aan de beurt");
            }

            this.Bord.VoegSchijfToe(kolom, spelerDieDeZetDoet.Kleur);

            // event dat er een set is gedaan.
            if (this.Bord.WelkKleurHeeft4op1Rij() != SpeelKleur.NietGezet)
            {
                // event wat het spel beëindigd met welke gegevens;
                this.BeëindigSpel(SpelStatus.Beeindigd4Op1Rij);
            }

            // misschien nog wel iets doen als het bord vol is en er geen winnaar is.
            //if (this.Bord.IsVol())
            //{
            //this.BeëindigSpel(SpelStatus.BeeindigdBordVol);
            // event spel beëindigd, maar dan met gelijkspel
            //}

            // ok dan moet ik dus de ander speler de huidige maken
            this.HuidigeSpeler = this.GetVolgendeSpeler(spelerDieDeZetDoet);

            // gooi een event dat de beurt om is.
            this.BeurtOver?.Invoke(this, this.HuidigeSpeler);
        }

        public void BeëindigSpelHandmatig()
        {
            this.BeëindigSpel(SpelStatus.BeeindigdDoorSpeler);
        }

        private void BeëindigSpel(SpelStatus status)
        {
            this.Status = status;
        }
    }
}

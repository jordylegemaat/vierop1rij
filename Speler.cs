// <copyright file="Speler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace Vierop1Rij.Domein
{
    using System;

    /// <summary>
    /// Class Speler.
    /// </summary>
    public class Speler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Speler"/> class.
        /// </summary>
        /// <param name="naam">kleur parameter.</param>
        /// <param name="kleur">naam parameter.</param>
        public Speler(string naam, SpeelKleur kleur)
        {
            this.Naam = !string.IsNullOrWhiteSpace(naam) ? naam : throw new ArgumentNullException(nameof(naam));

            if (kleur == SpeelKleur.NietGezet)
            {
                throw new ArgumentNullException(nameof(kleur));
            }

            this.Kleur = kleur;
        }

        /// <summary>
        /// Gets de Naam.
        /// </summary>
        public string Naam { get; }

        /// <summary>
        /// Gets de Kleur waarmee de speler speelt.
        /// </summary>
        public SpeelKleur Kleur { get; }
    }
}

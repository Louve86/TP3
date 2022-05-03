using System;

namespace Mediatek86.metier
{
    /// <summary>
    /// Class qui gère le constructeur d'AbonnementRevue
    /// </summary>
    public class AbonnementRevue
    {
        private readonly string id;
        private readonly DateTime dateCommande;
        private readonly DateTime dateFinAbonnement;
        private readonly string idRevue;
        private readonly bool empruntable;
        private readonly string titre;
        private readonly string periodicite;
        private readonly int delaiMiseDispo;
        private readonly string genre;
        private readonly string publicdoc;
        private readonly string rayon;
        private readonly string image;
        private readonly double montant;

        /// <summary>
        /// Le constructeur
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dateCommande"></param>
        /// <param name="dateFinAbonnement"></param>
        /// <param name="idRevue"></param>
        /// <param name="empruntable"></param>
        /// <param name="titre"></param>
        /// <param name="periodicite"></param>
        /// <param name="delaiMiseDispo"></param>
        /// <param name="genre"></param>
        /// <param name="publicdoc"></param>
        /// <param name="rayon"></param>
        /// <param name="image"></param>
        /// <param name="montant"></param>
        public AbonnementRevue(string id, DateTime dateCommande, DateTime dateFinAbonnement, string idRevue,
            bool empruntable, string titre, string periodicite, int delaiMiseDispo, string genre, string publicdoc, string rayon,
            string image, double montant)
        {
            this.id = id;
            this.dateCommande = dateCommande;
            this.dateFinAbonnement = dateFinAbonnement;
            this.idRevue = idRevue;
            this.empruntable = empruntable;
            this.titre = titre;
            this.periodicite = periodicite;
            this.delaiMiseDispo = delaiMiseDispo;
            this.genre = genre;
            this.publicdoc = publicdoc;
            this.rayon = rayon;
            this.image = image;
            this.montant = montant;
        }

        /// <summary>
        /// Recupere l'ID de la commande
        /// </summary>
        public string Id { get => id; }
        /// <summary>
        /// Recupere la date de commande (date début abonnement)
        /// </summary>
        public DateTime DateDeCommande { get => dateCommande; }
        /// <summary>
        /// Recupere la date de fin d'abonnement
        /// </summary>
        public DateTime DateDeFinAbonnement { get => dateFinAbonnement; }
        /// <summary>
        /// Recupere l'id de la revue
        /// </summary>
        public string IdRevue { get => idRevue; }
        /// <summary>
        /// Recupere le status d'empruntabilité
        /// </summary>
        public bool Empruntable { get => empruntable; }
        /// <summary>
        /// Recupere le titre
        /// </summary>
        public string Titre { get => titre; }
        /// <summary>
        /// Recupere la periodocité
        /// </summary>
        public string Periodicite { get => periodicite; }
        /// <summary>
        /// Recupere le delai de mise à dispo
        /// </summary>
        public int DelaiMiseDispo { get => delaiMiseDispo; }
        /// <summary>
        /// Recupere le genre
        /// </summary>
        public string Genre { get => genre; }
        /// <summary>
        /// Recupere le public
        /// </summary>
        public string Public { get => publicdoc; }
        /// <summary>
        /// Recupere le rayon
        /// </summary>
        public string Rayon { get => rayon; }
        /// <summary>
        /// Recupere l'image
        /// </summary>
        public string Image { get => image; }
        /// <summary>
        /// Recupere le montant en ajoutant € a la fin
        /// </summary>
        public string Montant { get => montant + "€"; }
    }
}

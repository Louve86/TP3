using System;

namespace Mediatek86.metier
{
    /// <summary>
    /// Class qui gère le constructeur de CommandeDocumentDvd
    /// </summary>
    public class CommandeDocumentDvd
    {
        private readonly string id;
        private readonly DateTime date;
        private readonly double montant;
        private readonly int nombreExemplaire;
        private readonly string idLivDVD;
        private readonly string idSuivi;
        private readonly string libelleSuivi;
        private readonly int duree;
        private readonly string titre;
        private readonly string realisateur;
        private readonly string synopsis;
        private readonly string genre;
        private readonly string publicdoc;
        private readonly string rayon;
        private readonly string image;

        /// <summary>
        /// Le constructeur
        /// </summary>
        /// <param name="id"></param>
        /// <param name="date"></param>
        /// <param name="montant"></param>
        /// <param name="nombreExemplaire"></param>
        /// <param name="idLivDVD"></param>
        /// <param name="idSuivi"></param>
        /// <param name="libelleSuivi"></param>
        /// <param name="duree"></param>
        /// <param name="titre"></param>
        /// <param name="realisateur"></param>
        /// <param name="synopsis"></param>
        /// <param name="genre"></param>
        /// <param name="publicdoc"></param>
        /// <param name="rayon"></param>
        /// <param name="image"></param>
        public CommandeDocumentDvd(string id, DateTime date, double montant, int nombreExemplaire,
            string idLivDVD, string idSuivi, string libelleSuivi, int duree, string titre,
            string realisateur, string synopsis, string genre, string publicdoc, string rayon, string image)
        {
            this.id = id;
            this.date = date;
            this.montant = montant;
            this.nombreExemplaire = nombreExemplaire;
            this.idLivDVD = idLivDVD;
            this.idSuivi = idSuivi;
            this.libelleSuivi = libelleSuivi;
            this.duree = duree;
            this.titre = titre;
            this.realisateur = realisateur;
            this.synopsis = synopsis;
            this.genre = genre;
            this.publicdoc = publicdoc;
            this.rayon = rayon;
            this.image = image;
        }

        /// <summary>
        /// Recupere l'id de commande
        /// </summary>
        public string Id { get => id; }
        /// <summary>
        /// Recupere la date de commande
        /// </summary>
        public DateTime DateDeCommande { get => date; }
        /// <summary>
        /// Recupere le montant en ajoutant un € après
        /// </summary>
        public string Montant { get => montant + "€"; }
        /// <summary>
        /// Recupere le nombre d'exemplaires 
        /// </summary>
        public int NombreExemplaire { get => nombreExemplaire; }
        /// <summary>
        /// Recupere l'id 
        /// </summary>
        public string IdLivDVD { get => idLivDVD; }
        /// <summary>
        /// Recupere l'id de l'étape de suivi 
        /// </summary>
        public string IdSuivi { get => idSuivi; }
        /// <summary>
        /// Recupere le libelle du suivi 
        /// </summary>
        public string Etat { get => libelleSuivi; }
        /// <summary>
        /// Recupere la duree 
        /// </summary>
        public int Duree { get => duree; }
        /// <summary>
        /// Recupere le titre 
        /// </summary>
        public string Titre { get => titre; }
        /// <summary>
        /// Recupere le/la réalisateur/trice
        /// </summary>
        public string Realisateur { get => realisateur; }
        /// <summary>
        /// Recupere le synopsis 
        /// </summary>
        public string Synopsis { get => synopsis; }
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
    }
}

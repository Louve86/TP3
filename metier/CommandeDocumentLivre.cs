using System;

namespace Mediatek86.metier
{
    /// <summary>
    /// Class qui gère le constructeur de CommandeDocumentLivre
    /// </summary>
    public class CommandeDocumentLivre
    {
        private readonly string id;
        private readonly DateTime date;
        private readonly double montant;
        private readonly int nombreExemplaire;
        private readonly string idLivDVD;
        private readonly string idSuivi;
        private readonly string libelleSuivi;
        private readonly string isbn;
        private readonly string titre;
        private readonly string auteur;
        private readonly string collection;
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
        /// <param name="isbn"></param>
        /// <param name="titre"></param>
        /// <param name="auteur"></param>
        /// <param name="collection"></param>
        /// <param name="genre"></param>
        /// <param name="publicdoc"></param>
        /// <param name="rayon"></param>
        /// <param name="image"></param>
        public CommandeDocumentLivre(string id, DateTime date, double montant, int nombreExemplaire,
            string idLivDVD, string idSuivi, string libelleSuivi, string isbn, string titre,
            string auteur, string collection, string genre, string publicdoc, string rayon, string image)
        {
            this.id = id;
            this.date = date;
            this.montant = montant;
            this.nombreExemplaire = nombreExemplaire;
            this.idLivDVD = idLivDVD;
            this.idSuivi = idSuivi;
            this.libelleSuivi = libelleSuivi;
            this.isbn = isbn;
            this.titre = titre;
            this.auteur = auteur;
            this.collection = collection;
            this.genre = genre;
            this.publicdoc = publicdoc;
            this.rayon = rayon;
            this.image = image;
        }

        /// <summary>
        /// Recupere l'id de la commande
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
        /// Recupere la date de commande 
        /// </summary>
        public int NombreExemplaire { get => nombreExemplaire; }
        /// <summary>
        /// Recupere l'id du Livre
        /// </summary>
        public string IdLivDVD { get => idLivDVD; }
        /// <summary>
        /// Recupere l'id de l'etape de suivi
        /// </summary>
        public string IdSuivi { get => idSuivi; }
        /// <summary>
        /// Recupere le libelle
        /// </summary>
        public string Etat { get => libelleSuivi; }
        /// <summary>
        /// Recupere le bumero ISBN
        /// </summary>
        public string ISBN { get => isbn; }
        /// <summary>
        /// Recupere le titre
        /// </summary>
        public string Titre { get => titre; }
        /// <summary>
        /// Recupere l'auteur/trice
        /// </summary>
        public string Auteur { get => auteur; }
        /// <summary>
        /// Recupere la collection
        /// </summary>
        public string Collection { get => collection; }
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

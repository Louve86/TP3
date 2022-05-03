using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mediatek86.metier
{
    /// <summary>
    /// Permet de connaitre l'état de la commande
    /// </summary>
    public class Suivi
    {
        /// <summary>
        /// Identifiant de l'état pour la commande
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Libellé de la commande
        /// </summary>
        public string Libelle { get; }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="id"></param>
        /// <param name="libelle"></param>
        public Suivi(string id, string libelle)
        {
            Id = id;
            Libelle = libelle;
        }

        /// <summary>
        /// Retourne le libellé de l'élement
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Libelle;
        }
    }
}
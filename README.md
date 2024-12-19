# Projet_Ubisoft
première pensée pour ‘ressources limitée’ : 
-	jeu de ‘survie’ ou il faut trouver de l’eau et de la nourriture pour ne pas mourir
-	jeu de gestion avec de la création ( jeu de parc, style bac à sable…)
-	jeu de reproduction d’écosystème

Idée principale :
Un jeu de gestion stratégique en vision isométrique basé sur l’évolution d’un écosystème. Un jeu calme sans ennemis ni PVP.
________________________________________________________________________________
But du Jeu
Le joueur débute sur une zone hostile, son but est de faire évoluer l’environnement pour crée un écosystème et attiré une faune. Chaque action coûte de l’énergie primaire (une seule énergie limitée pour toute les actions) qu’il faut utiliser de manière réfléchie afin d’optimiser les zones habitables. La faune apparait lorsque toute les conditions sont réunis (condition différentes pour chaque espèces) et permet de régénérer l’énergie pour continuer. Un % représente la réussite du joueur, chaque évolution ou animal en plus le fait augmenter, 100% est atteint lorsqu’il y a toutes les espèces d’animaux.
Inspirations :
- Terra Nil : https://www.youtube.com/watch?v=vqYpaVoiymo
- World box
- Jeu de la vie ( pour les évolutions du jeu )
- Jeux en vue isométrique : Terra Nil, Dofus (sans changement de map)
________________________________________________________________________________
Aspect techniques
-	Energie primaire limitée qui se régénère grâce à la faune
-	Action principale : faire évoluer une case : chaque case a une type assigner de base (terre, sable, roche...) qui se transformeront en case plus utile (champ, arbre..), chaque niveau d’évolution coute plus cher que le précédent.
-	Un ensemble de case crée des zones ( 5 case arbres = une foret), ces zone permette l’apparition des animaux.
-	Création d’une map unique au début de partie mais équilibré : génération procédurale simple
-	Pour choisir quelle case le joueur fait évoluer, il les sélectionne en cliquant avec sa souris, ou bien déplacement simple avec des flèches ( beaucoup mois pratique)
________________________________________________________________________________
Types de cases, évolutions et zones (exemple)
-	Terre stérile -> terre fertile -> végétation (zone champs)
-	Arbre mort -> arbre simple -> arbre fleuris (zone foret)
-	Eau stagnante -> eau clair -> eau potable (zone marrais, lac..)
-	Sable désertique -> sable -> terre sableuse (zone plage..)
-	Caillou -> roche -> roche mousseuse (zone montagne)
________________________________________________________________________________
Problèmes potentiels
-	Gestion des cases d’eau : si il n’y a que 1 bouton pour faire évoluer, on peux pas rajouter de l’eau. 
Solutions : mettre en place des intempéries, rajouter des cases aléatoirement, faire étendre les cases d’eau, rajouter un autre bouton pour faire apparaitre de l’eau (respect du sujet ?)
-	Peu de difficulté : une seule action peut rendre le jeu trop ennuyant
Solution : bon équilibre de la ressource primaire pour que se soit jouable tout en forçant le joueur a réfléchir, ajouter des animaux divers qui peuvent partir si leur zone n’est pas bien géré (le % peut redescendre)
-	Difficulté a créé les zones (car on ne peut pas changer le type de base des cases)
Solution : mettre en place un système de vie de l’écosystème : si 3 cases du même type : la 4 -ème dévient de ce type : jeu de la vie), l’eau peut faire évoluer les cases voisines
________________________________________________________________________________
Étapes 
-	Génération de la map automatique simple
o	Deux versions faite : une en créant une Tilemap avec une palette, l’autre en créant des games objects. Deuxième version me permet de plus facilement géré les cases mais la première semble plus optimisé.
-	Mise en place de la sélection de cases (plus simple avec des games objects)
-	UI (avec gestion de la ressources et % de réussite)
-	Mise en place du bouton pour évoluer les cases
-	Gestion des zones
-	Spawn des animaux
Évolution en +
-	Menu (début pause fin…)
-	Gestion de la caméra ( en fonction de la taille de la map)
-	Création de l’esthétique des cases (certaines faite)
-	Génération procédurale ( peu être fait plus tard )
-	Mise en place des intempéries
-	Système de vie « autonome » de l’écosystème (jeu de la vie)
-	IA simple pour le déplacement des animaux
-	Système de sauvegarde des maps
-	Bouton retour en arrière de l’action réaliser
-	Mettre plus d’action possible et de bouton (par exemple interagir avec les animaux…)


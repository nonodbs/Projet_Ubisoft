# Projet_Ubisoft

Idée principale :
Un jeu de gestion stratégique en vision isométrique basé sur l’évolution d’un écosystème. Un jeu calme sans ennemis ni PVP.
________________________________________________________________________________

But du Jeu :
Le joueur débute sur une zone hostile, son but est de faire évoluer l’environnement pour crée un écosystème et attiré une faune. Chaque action coûte de l’énergie primaire (une seule énergie limitée pour toute les actions) qu’il faut utiliser de manière réfléchie afin d’optimiser les zones habitables. La faune apparait lorsque toute les conditions sont réunis (condition différentes pour chaque espèces) et permet de régénérer l’énergie pour continuer. Un % représente la réussite du joueur, chaque évolution ou animal en plus le fait augmenter, 100% est atteint lorsqu’il y a toutes les espèces d’animaux.

Inspirations :
- Terra Nil : https://www.youtube.com/watch?v=vqYpaVoiymo
- World box
- Jeu de la vie ( pour les évolutions du jeu )
- Jeux en vue isométrique : Terra Nil, Dofus (sans changement de map)

________________________________________________________________________________

Comment jouer :
- télécharger le dossier V1 (au complet), et lancer l'application "Mygame"
- bouton gauche de la souris : cliquer sur les boutons, selection des cases, déselection des cases
- espace (clic court) : fait évoluer les cases sélectionnées
- espace (clic long) : change les cases selectionnées en eau
- bouton droit de la souris : recentre la caméra sur le joueur
- echap : mets en pause le jeu
  
________________________________________________________________________________

Mécaniques principales :
-	Ressource centrale : Une énergie primaire limitée, utilisée pour toutes les actions du jeu. Cette énergie se régénère au fur et à mesure que la faune apparait.
-	Évolution des cases : Le joueur peut sélectionner des cases de terrain ayant un type de base (terre, sable, roche, etc.) et les faire évoluer en cases plus utiles (champs, forêts, lacs...). Chaque niveau d'évolution coûte davantage d’énergie que le précédent.
-	Création de zones : Un ensemble de cases évoluées crée des zones spécifiques (par exemple, 6 cases d'arbres forment une forêt). Ces zones permettent l'apparition de la faune, qui contribuent à la régénération de l'énergie.
-	Génération de la map : Une génération procédurale simple est utilisée au début de chaque partie pour créer une carte unique mais équilibrée, offrant des défis variés. (Version future)
-	Sélection intuitive des cases : Le joueur utilise un système de clic à la souris pour sélectionner les cases à faire évoluer.
-	Écosystème autonome : L'environnement évolue automatiquement grâce à un système de vie dynamique. Par exemple, certaines cases peuvent se transformer naturellement en fonction de leurs voisines, comme dans un "jeu de la vie". (Version futur)

________________________________________________________________________________

Version 1 :
-	Génération de la map automatique simple (random)
-	Mise en place de la sélection de cases
-	UI (avec gestion de la ressources et % de réussite)
-	Mise en place du bouton pour évoluer les cases
-	Gestion des zones
-	Spawn des animaux, et déplacement
-	Menu (Début, pause, fin)
-	Gestion simple de la caméra
-	Création de l'esthétique des cases

[Télécharger la vidéo](Vidéo/08012025_V1.mp4)

________________________________________________________________________________

Évolutions future :
-	Génération procédurale
-	Système de vie « autonome » de l’écosystème (jeu de la vie)
-	Rajout d'animaux variés
-	Amélioration de la caméra
-	Amélioration des esthétiques des menus, fond..
-	Rajout de son

Évolutions possibles :
- Mise en place d'intempéries
-	Système de sauvegarde des maps
-	Bouton retour en arrière de l’action réaliser
-	Interaction avec les animaux

________________________________________________________________________________

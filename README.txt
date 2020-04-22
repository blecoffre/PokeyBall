On ne peut drag que sur le joueur afin de le propulser vers le haut, 
la propulsion vers le bas n'est pas bloqué dans l'état actuelle du projet

En ce qui concerne mes priorités :

- J'ai priorisé le PlayerController, étant le core gameplay.
- Une fois les contrôles suffisant pour tester le gameplay, j'ai décider d'ajouter une condition de fin de niveau : la ligne d'arrivée, 
qui lance le prochain directement.
- Ensuite j'ai décider d'inclure les Obstacles (zone ou on ne peut pas s'accrocher) afin d'ajouter une brique de gameplay

Pour ce qui est de mes choix :
- J'ai utilisé le spring joint car il m'a semblé que c'était le joint le plus approprié au gameplay voulu.
- J'ai utilisé une simple image en guise de background afin que le joueur est la sensation que la balle se déplace.
- Le chargement abrupte du niveau suivant une fois la ligne d'arrivé atteinte est un choix, n'ayant pas mis en place de système de score / pièce
comme dans le jeu original j'ai préféré coupé court pour le moment.


Si je devais m'auto évaluer, je dirais que mon travail représente bien ce qu'est un protoype :
Il n'est pas exempt de bug, manque de polish, mais l'intention est la :
	Le core gameplay peut être testé et compris par l'utilisateur, il y a une condition de victoire et de défaite : touché le sol et perdre, atteindre la ligne d'arrivée et passer au
niveau suivant.
	Et l'on peut ajouter d'autres brique autour de ce core gameplay afin de l'agrémnter et le rendre plus fun / intéressant.
L'équilibrage n'est pas encore tout à fait au point : La force que l'on peut donner à la balle est parfois bien trop grande

Le rendu visuel n'est pas exceptionnelle, le but étant simplement d'avoir des éléments de repères : le fond pour le mouvement de la balle, et la ligne d'arrivée pour représenter un objectif
à atteindre.

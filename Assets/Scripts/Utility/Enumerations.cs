using UnityEngine;

// SECTION - Enumeration ============================================================

// Basic Ennemy
public enum BasicEnemy_States { ONE, TWO }

public enum BasicEnemy_AnimationStates { IDDLE, MOVEMENT, ONAWAKE, STATE_ONE_ATTACK, STATE_TWO_ATTACK, DEAD }

public enum BasicEnemy_AnimTriggers { DEATH, EXITDEATH, ONHIT, STATEONEATTACK, STATETWOATTACK}


// Base Enemy AI
public enum ValidationCheckTypes { CHILDSPECIFIC, ALWAYSVALID, RAYCASTSINGLE, RAYCASTARRAY, OVERLAPSPHERE }

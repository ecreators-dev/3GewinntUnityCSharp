# 3Gewinnt
Unity
C#

# Components

**Board** (MonoBehaviour)
* Spalten
* Zeilen
* Abstand
* Pool

**Zelle** (MonoBehaviour)
* Index
* Content : ZellInhalt

**ZellInhalt** (MonoBehaviour)
* Cell : Zelle

**Challenge** (ScriptableObject) [AssetMenu]
* ChallengeEntry[]
* Turns

**ChallengeEntry** (ScriptableObject) [AssetMenu]
* Amount
* Total
* Content : ZellInhalt

**Vorlagen** (Prefab)
* Zelle
* ZellInhalt

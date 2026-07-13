=== Kap ===

{Kap > 1: -> CapKid_Repeat}

#nm_Kap #d_kap
Have you seem my dad? 

#d_kap
He told me that he would buy me <wave a=0.5 s=0.5><rainb s=0.5>ballons for my birthday</rainb></wave>.

{IsQuestCompleted("found_balloons"): -> CapKid_Win}

#d_kap
I hope he kept his promise...

-> END

=== CapKid_Repeat ===

#nm_Kap #d_kap
{IsQuestCompleted("found_balloons"): -> CapKid_Win}
<wave a=1.25 s=0.5>I miss my dad...</wave> #d_kap

-> END

=== CapKid_Win ===

#nm_Kap #d_kap
{CapKid_Win > 1: -> CapKid_WinRepeat}
<shake>Oh!</shake>

#d_kap
You found some ballons for me!

 #d_kap
As a reward, have this shovel that my dad told me to keep safe for him.

 #d_kap
I don't think he's comming back...

-> END

=== CapKid_WinRepeat ===

#nm_Kap  #d_kap
Tell me if you see my dad anywhere.

-> END
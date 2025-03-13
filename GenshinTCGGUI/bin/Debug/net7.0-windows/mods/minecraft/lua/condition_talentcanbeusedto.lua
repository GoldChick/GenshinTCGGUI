-- condition_talentcanbeusedto.lua |从sender中获取手牌信息，检测是否是[天赋]且能被使用于[我方]的[某个指定角色]
-- ============================

-- 某张牌能够对[附属有具有此条件的状态的角色]使用时为true
function This(me, p, s, v)
    if s.Persistent ~= nil then
        local card = s.Persistent.CardBase
        local character = me.Characters[p.Persistent]
        if card.Tags:Contains('Talent') and card.TargetDemands ~= nil then
            -- IEnumerable<list<>>
            local ienumerable_targetselections = me:GetTargetValid(card.TargetDemands)
            local list_targetselections = Enumerable.ToList(ienumerable_targetselections)
            for i = 0, list_targetselections.Count - 1, 1 do
                if list_targetselections[i]:Contains(character) then
                    return true
                end
            end
        end
    end
    return false
end

-- 某张牌能够对[当前出战的角色]使用时为true
function Curr(me, p, s, v)
    if s.Persistent ~= nil then
        local card = s.Persistent.CardBase
        local character = me.Characters[p.CurrCharacter]
        if card.Tags:Contains('Talent') and card.TargetDemands ~= nil then
            -- IEnumerable<list<>>
            local ienumerable_targetselections = me:GetTargetValid(card.TargetDemands)
            local list_targetselections = Enumerable.ToList(ienumerable_targetselections)
            for i = 0, list_targetselections.Count - 1, 1 do
                if list_targetselections[i]:Contains(character) then
                    return true
                end
            end
        end
    end
    return false
end

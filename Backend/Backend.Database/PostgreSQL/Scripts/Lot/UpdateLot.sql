UPDATE "Lots"
SET "name"        = @name,
    "description" = @description,
    "startPrice"  = @startPrice,
    "buyoutPrice" = @buyoutPrice,
    "betStep"     = @betStep,
    "state"       = @state
WHERE "id" = @id;
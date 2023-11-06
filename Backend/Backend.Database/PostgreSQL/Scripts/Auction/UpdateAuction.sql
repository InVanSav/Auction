UPDATE "Auctions"
SET "name"        = @name,
    "description" = @description,
    "dateStart"   = @dateStart,
    "dateEnd"     = @dateEnd,
    "state"       = @state
WHERE "id" = @id;
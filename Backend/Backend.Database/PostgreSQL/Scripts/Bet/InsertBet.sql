INSERT INTO "Bets" ("id", "value", "lotId", "userId", "dateTime")
VALUES (@id, @value, @lotId, @userId, @dateTime) ON CONFLICT DO NOTHING;
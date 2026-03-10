-- AppUsers seed data
-- Seed admin user for local development (password: "admin")
INSERT INTO app."AppUsers" ("Email", "PasswordHash", "Name", "Role")
VALUES (
    'admin@admin.com',
    '$2a$11$ME3Q9eYEI3IBhokEgLICeeqEQt66mdV.eqwW9rq5GMLYme4cd6VS.',
    'Admin',
    'Admin'
)
ON CONFLICT ("Email") DO NOTHING;

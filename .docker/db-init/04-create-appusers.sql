-- AppUsers table for authentication
-- Supports both password-based registration and Google OAuth login.

CREATE TABLE IF NOT EXISTS app."AppUsers" (
    "Email"        VARCHAR(250) PRIMARY KEY,
    "PasswordHash" VARCHAR(500) NULL,
    "Name"         VARCHAR(100) NULL,
    "Role"         VARCHAR(50)  NOT NULL DEFAULT 'User',
    "CreatedAt"    TIMESTAMP    NOT NULL DEFAULT NOW()
);

-- Seed admin user for local development (password: "admin")
INSERT INTO app."AppUsers" ("Email", "PasswordHash", "Name", "Role")
VALUES (
    'admin@admin.com',
    '$2a$11$ME3Q9eYEI3IBhokEgLICeeqEQt66mdV.eqwW9rq5GMLYme4cd6VS.',
    'Admin',
    'Admin'
)
ON CONFLICT ("Email") DO NOTHING;

﻿CREATE TABLE @Schema.voter
(
    id uuid NOT NULL,
    pesel character varying(16) NOT NULL,
    is_administrator boolean NOT NULL,
    PRIMARY KEY (id)
);
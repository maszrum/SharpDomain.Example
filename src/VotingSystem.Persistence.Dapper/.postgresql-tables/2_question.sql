CREATE TABLE @Schema.question
(
    id uuid NOT NULL,
    question_text character varying(256) NOT NULL,
    PRIMARY KEY (id)
);
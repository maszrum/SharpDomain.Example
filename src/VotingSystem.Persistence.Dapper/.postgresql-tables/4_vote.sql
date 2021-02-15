CREATE TABLE @Schema.vote
(
    id uuid NOT NULL,
    voter_id uuid NOT NULL,
    question_id uuid NOT NULL,
    PRIMARY KEY (id),
    CONSTRAINT voter_id_fkey FOREIGN KEY (voter_id)
        REFERENCES @Schema.voter (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID,
    CONSTRAINT question_id_fkey FOREIGN KEY (question_id)
        REFERENCES @Schema.question (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID
);
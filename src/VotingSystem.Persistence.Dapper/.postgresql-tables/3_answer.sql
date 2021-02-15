CREATE TABLE @Schema.answer
(
    id uuid NOT NULL,
    question_id uuid NOT NULL,
    answer_order integer NOT NULL,
    text character varying(256) NOT NULL,
    votes integer NOT NULL,
    PRIMARY KEY (id),
    CONSTRAINT question_id_fkey FOREIGN KEY (question_id)
        REFERENCES @Schema.question (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID
);
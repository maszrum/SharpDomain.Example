CREATE TABLE public.answer
(
    id uuid NOT NULL,
    question_id uuid NOT NULL,
    "order" integer NOT NULL,
    text character varying(256) NOT NULL,
    votes integer NOT NULL,
    PRIMARY KEY (id),
    CONSTRAINT question_id_fkey FOREIGN KEY (question_id)
        REFERENCES public.question (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID
);

ALTER TABLE public.answer
    OWNER to db_user;
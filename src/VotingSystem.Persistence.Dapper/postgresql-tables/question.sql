CREATE TABLE public.question
(
    id uuid NOT NULL,
    question_text character varying(256) NOT NULL,
    PRIMARY KEY (id)
);

ALTER TABLE public.question
    OWNER to db_user;
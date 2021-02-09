## PostgreSQL database docker-compose file

### Docker-compose commands
- to create containers and start services:
``docker-compose up -d``

- to stop services and remove containers:
``docker-compose down``

- to start services
``docker-compose start``

- to stop services:
``docker-compose stop``

### Access to PostgreSQL database
- Database name: ``voting_system``
- Port: ``5432``
- User: ``db_user``
- Password: ``db_pass``

### Access to PgAdmin
- URL: ``http://localhost:8080``
- E-mail: ``admin@admin.com``
- Password: ``changeme``

Add new server to pgAdmin with:
- Host name/address: ``db_postgres``
- Port: ``5432``
- Username: ``db_user``
- Password: ``db_pass``
script_dir = ./scripts
migration_name ?= new_migration

# Makes all bash scripts excutable
activate-scripts:
	chmod +x $(script_dir)/Activate-Script.sh
	$(script_dir)/Activate-Script.sh


# Restore (download) all nuget packages
restore:
	$(script_dir)/Restore.sh


# Builds the project
build:
	$(script_dir)/Restore.sh


# Runs the project
run:
	$(script_dir)/Run.sh


# Publish - prepares application for deplyment
publish:
	$(script_dir)/Publish.sh


# Adds migrations
migrate:
	$(script_dir)/Migrate.sh $(migration_name)


# Deletes migration
remove-migration:
	$(script_dir)/Remove-Migration.sh


# Updates database schema
update-database:
	$(script_dir)/Update-Database.sh
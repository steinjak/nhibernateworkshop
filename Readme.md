NHibernateWorkshop
==================
A sample program used as a testbench during an NHibernate workshop on SPS@2013-11-25.

To get started:
* Create a database "NHibernateWorkshop" in (localdb)\Projects
* Run the "test" NHibernateWorkshop.Tests\Utils\GenerateSchemaFromMappings::InstallSchemaFromMappingsToDb
* Run the "test" NHibernateWorkshop.Tests\Utils\GenerateData::Generate to install example data
* Fire up NHibernate Profiler
* Start the application normally (Ctrl+F5)

Switch between FluentNHibernate ClassMaps and Mapping-by-code by editing SessionProvider.cs;
there's a bool that can be flipped to choose what mappings to use.

Things to try:
* The Read action needs attention, as it has severe SELECT N+1-problems - it should be possible to run this action with a constant number of queries.
* Try experimenting with the cache, both the entity cache (turn it on in the mappings) and the query cache (use .Cacheable() on the query)
* Play with the mappings and try to break things, for instance: Create an action or a test that adds comments to a post and experiment with
  the inverseness of the Comments collection, cascading, etc

Disclaimer: Many shortcuts have been taken, as this workbench focuses on data access only. Error handling/logging is minimal, the entities
provide no encapsulation or behaviour whatsoever and are directly exposed to the views, the transactions run longer than necessary, etc.
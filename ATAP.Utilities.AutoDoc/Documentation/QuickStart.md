# AutoDoc project QuickStart
This project creates a static website of documentation out of the other projects in a repository.
It depends on DocFx. 
It is based on Triple Slash (///) documentation in the .cvs files, and .md documents in a Docs directory in any project you want to documentation
It creates a static site.
This site can be hosted on GitHub Pages.

Versioning:
GitHub lets you publish from either your `master`branch or your `gh-pages` branch. Since master should always build and pass tests, and because the developers and moderators/architects should be able to merge into master anytime, it makes better sense to use the gh-pages branch to host the static website. When a release branch is created from master to release a version of code, then while the release code is going through testing, the documentation should be going through tests too, to ensure the new code is properly documented. testing cannot be doe on the production website `gh-pages`. So we use the docfx --serve option to create teh website locally, and to test it locally and someday in a CI pipeline too, I hope.
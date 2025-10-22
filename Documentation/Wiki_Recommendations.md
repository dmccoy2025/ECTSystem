# Wiki Recommendations for ECTSystem

## Overview
The ECTSystem solution can benefit from integrated documentation via a wiki. Since the project is source-controlled on GitHub (repo: `dmccoy2025/ECTSystem`), a GitHub Pages-based wiki is recommended for simplicity, cost-effectiveness, and seamless integration. This provides a Markdown-based, collaborative documentation site hosted directly from the repository, mimicking Azure DevOps Wiki features like hierarchical pages, search, and version history.

## Recommended Approach: GitHub Pages Wiki
GitHub Pages allows hosting a static website from your repo's `docs/` folder or a `gh-pages` branch. Use Jekyll (GitHub's static site generator) for a wiki-like interface with navigation, themes, and basic search.

### Steps to Implement
1. **Create the Docs Folder**:
   - In the workspace root, create a `docs/` folder.
   - Add Markdown files for wiki pages (e.g., `docs/index.md` for the homepage, `docs/architecture.md` for system architecture).
   - Copy relevant content from the `Documentation/` folder (e.g., Architectural_Recommendations.md, RESTful_Recommendations.md).
   - Use relative links for navigation (e.g., `[Architecture](architecture.md)`).

2. **Add Jekyll for Wiki Features**:
   - Create `docs/_config.yml`:
     ```yaml
     title: ECTSystem Wiki
     description: Electronic Case Tracking System Documentation
     theme: minima  # Or 'jekyll-theme-cayman' for a wiki-like theme
     ```
   - Create `docs/Gemfile` for local testing:
     ```
     source 'https://rubygems.org'
     gem 'github-pages', group: :jekyll_plugins
     ```
   - Install Jekyll locally (requires Ruby): `gem install bundler jekyll`.
   - Test locally: `cd docs && bundle exec jekyll serve` (serves at http://localhost:4000).

3. **Enable GitHub Pages**:
   - Commit and push `docs/` to the `main` branch.
   - In GitHub repo settings (https://github.com/dmccoy2025/ECTSystem/settings/pages), set source to `main` branch with `/docs` folder.
   - The wiki will be available at `https://dmccoy2025.github.io/ECTSystem/`.

4. **Customize for Wiki Feel**:
   - Use Jekyll themes for navigation and layout.
   - Add a sidebar or TOC in `docs/_includes/sidebar.md`.
   - Enable search with plugins like `jekyll-feed` or Algolia.
   - Link to GitHub's version history for page edits.

5. **Integrate with Solution**:
   - Add wiki links in the Blazor client (e.g., in `AF.ECT.WebClient` navigation).
   - Update `README.md` with a "Documentation" link to the wiki.

## Azure DevOps-Style Wiki Approach
For an Azure DevOps-style wiki (Markdown-based, collaborative, with page trees, search, and version history), integrate a self-hosted Blazor Server project. This mimics Azure DevOps Wiki's features while keeping it .NET-native and orchestrable with Aspire.

### Steps to Implement
1. **Create the New Project**:
   - Create a Blazor Server project: `dotnet new blazorserver -n AF.ECT.Wiki -o AF.ECT.Wiki --framework net9.0`.
   - Add to solution: `dotnet sln add AF.ECT.Wiki/AF.ECT.Wiki.csproj`.

2. **Install NuGet Packages**:
   - Add to `AF.ECT.Wiki.csproj`:
     ```xml
     <PackageReference Include="Markdig" Version="0.37.0" />
     <PackageReference Include="Lucene.Net" Version="4.8.0-beta00016" />
     ```

3. **Integrate with .NET Aspire**:
   - In `AppHost.cs`: `var wiki = builder.AddProject<Projects.AF_ECT_Wiki>("wiki").WithExternalHttpEndpoints();`.

4. **Implement Wiki Features**:
   - Store Markdown in `AF.ECT.Wiki/WikiPages/` (hierarchical structure).
   - Use Markdig for rendering in `WikiPage.razor`.
   - Add sidebar navigation and search with Lucene.Net.
   - Enable editing with a Markdown editor.
   - Seed with content from `Documentation/`.

5. **Build and Run**:
   - `dotnet build ElectronicCaseTracking.sln`.
   - Launch via Aspire: Access at assigned endpoint.

This provides a dynamic, interactive wiki with Azure DevOps-like features.

## Pros and Cons
### GitHub Pages Wiki
- **Pros**:
  - Free and hosted by GitHub (no server costs).
  - Fully version-controlled alongside code in the repo.
  - No additional .NET project or build complexity.
  - Collaborative editing via GitHub pull requests and issues.
  - Easy to set up with Jekyll for themes and navigation.
- **Cons**:
  - Static content only (no dynamic features like user authentication or real-time editing).
  - Requires Jekyll knowledge for advanced customization (e.g., search, plugins).
  - Limited interactivity; relies on external services for features like comments.

### Azure DevOps-Style Wiki (Blazor Server)
- **Pros**:
  - Dynamic and interactive (e.g., in-page editing, user auth, custom workflows).
  - Full .NET integration with existing solution (Aspire orchestration, shared libraries).
  - Mimics Azure DevOps Wiki features (page trees, search, version history) closely.
  - Scalable and extensible with Blazor components.
- **Cons**:
  - More complex to implement and maintain (additional project, dependencies).
  - Requires server resources (not free like GitHub Pages).
  - Potential performance overhead for large wikis compared to static sites.

## Conclusion
Start with GitHub Pages for quick setup and low overhead. Expand to Blazor if dynamic features are needed. Ensure documentation is kept up-to-date alongside code changes.
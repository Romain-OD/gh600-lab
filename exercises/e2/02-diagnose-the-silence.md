# Step 2 — Diagnose a screen with no error on it

You have no error message. You cannot grep for a string that was never printed. So you work
the layers, outermost first, and ask one question at each: *did the tool exist here?*

## Question 1 — does the server work at all, without the agent?

Drop the agent and ask the same CLI the same kind of question:

```bash
copilot -p "Without calling anything, list the names of every MCP tool registered in this session, grouped by server." --allow-all-tools
```

You will get a list, and `github-mcp-server` will be in it:

```
github-mcp-server: get_copilot_space, get_file_contents, list_copilot_spaces,
                   search_code, search_users
```

So the server is fine, the CLI is fine, and your login is fine. **The tools exist right up
until the agent is applied.** One flag is the difference. That is your bisect, and it took
one command.

## Question 2 — what does the agent actually receive?

Put the agent back and ask it directly:

```bash
copilot -p "Without calling anything, list the exact names of every MCP tool available to you, one per line." --agent release-notes --allow-all-tools
```

Nothing. Not a shortened list — an empty one.

That is the finding that matters, and it is worth being precise about what it rules out.
You did not get *some* of the tools you asked for. You got **none** of them, including ones
you spelled out individually. A partially-honoured allow list would suggest a permission
problem. A completely-ignored one suggests the entries were never understood in the first
place.

## Question 3 — what does the product do with a line it does not understand?

This is the documented behaviour, and it is the whole bug
([custom agents configuration reference][ref], § Tools):

> "All unrecognized tool names are ignored, which allows product-specific tools to be
> specified in an agent profile without causing problems."

Read that twice. It is a deliberate, sensible design decision — an agent file should survive
being opened by a different product that has different tools — and it means your allow list
has no validation whatsoever. `github(search_code)` is not a syntax error. It is an entry
for a tool that does not exist, discarded exactly as designed.

Strip the entries the product understood and here is what your agent was actually configured
with:

```yaml
tools: ['read', 'search']          # what the CLI saw
```

No MCP entries at all. And an allow list with no entry for a tool is a closed door, so the
GitHub MCP tools were never registered, so the model was never offered them.

## Why `--allow-all-tools` could not help

Because it operates on a different gate.

| | Registration | Permission |
|---|---|---|
| Question it answers | Does this tool **exist** for this agent? | May this existing tool **run**? |
| Set by | MCP config, agent file `tools:` | `--allow-tool`, `--deny-tool`, `/permissions`, runtime prompts |
| Failure looks like | Silence | A visible `✗` naming the rule |

`--allow-all-tools` grants permission to every tool that exists. Zero MCP tools existed, so
it granted permission to nothing. **Permissions cannot resurrect a tool that was never
registered** — and an exam question that offers you a broader permission as the fix for a
missing tool is testing exactly this.

## The diagnosis

One line, four entries, two of them silently discarded:

```yaml
tools: ['read', 'search', 'github(search_code)', 'github(get_file_contents)']
#                          ^^^^^^^^^^^^^^^^^^^  parentheses: permission syntax, not
#                                               registration syntax — ignored
```

Step 3 fixes it.

[ref]: https://docs.github.com/en/copilot/reference/custom-agents-configuration

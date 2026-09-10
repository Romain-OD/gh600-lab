# The ruleset — gate the risk, not the activity

**Settings → Rules → Rulesets → New ruleset → New branch ruleset.**

| Setting | Value |
|---|---|
| Ruleset name | `protect-default` |
| Enforcement status | Active |
| Target branches | Include default branch |
| Restrict deletions | on |
| Require a pull request before merging | on |
| — Required approvals | 1 |
| — Dismiss stale approvals on push | on |
| — Require review from Code Owners | off |
| Require status checks to pass | on → add `test` |
| Block force pushes | on |

Leave **Bypass list** empty. An agent, or a human, with bypass is the whole control undone.

## What this does and does not do

The agent can still plan, write code, push branches, open a pull request and run the checks
entirely on its own. Nothing about its speed changed.

What it cannot do is land anything on the default branch by itself, because the one approval has to come
from somebody who is not the author.

## The part that is easy to get wrong

Notice what is **not** gated:

- not every action
- not every commit
- not every file

Only the merge. Merge is the irreversible step; everything before it is a branch you can
delete. That is the whole heuristic — **grade each action by how expensive it is to undo,
and put the control there.**

A README typo fix on a branch still goes from idea to merged in under a minute. If your
version does not, you have built a bottleneck and the exam's phrase for it is *significantly
slowing delivery*.

## Prove it

```bash
# 1. the negative test — must be refused
#    ask the agent to merge its own PR
#    expected: "Changes must be approved by a reviewer other than the author"

# 2. the speed test — must still be fast
git checkout -b docs/typo
# fix one word in README.md
git commit -am "docs: fix typo"
git push -u origin docs/typo
gh pr create --fill
gh pr merge --squash --admin=false   # approve from a second account, or merge after review
```

Both have to be true. A gate that only blocks is not a gate, it is a wall.

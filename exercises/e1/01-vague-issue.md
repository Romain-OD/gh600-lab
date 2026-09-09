# Run one — the vague issue

Paste this as the issue title and body. Do not improve it. The whole point is that it is
the kind of task people really do hand over.

---

**Title**

```
Clean up the payment module
```

**Body**

```
The payment module has grown messy. Please clean it up.
```

---

Assign it to Copilot and leave it alone.

## What to expect

A pull request with somewhere around a dozen changed files: a renamed interface, the
hand-rolled serializer swapped out, the `Legacy/` folder deleted or rewritten, naming
normalised across the request and result types — and, somewhere in the middle, one
genuinely good fix.

Nothing in it is *wrong*. That is what makes it unreviewable.

Record the exact **files changed** and **lines changed** numbers before you close it.

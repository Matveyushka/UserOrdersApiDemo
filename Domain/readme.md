# Domain

On the one hand, it is good practice to make domain entities POCO. On the other hand:

- I need them to inherit a common interface IEntity to make things generic

- it is convenient to use `DataAnnotations` attributes for domain-level validation
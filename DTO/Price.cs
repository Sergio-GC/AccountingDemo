﻿namespace DTO
{
    public class Price
    {
        public int Id { get; set; }
        public required string Label { get; set; }
        public required float Value { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}

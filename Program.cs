//*****************************************************************************
//** 884. Uncommon Words from Two Sentences   leetcode                       **
//*****************************************************************************


/**
 * Note: The returned array must be malloced, assume caller calls free().
 */
// Define a structure for hash map
typedef struct {
    char* word;
    int count;
} HashMapEntry;

typedef struct {
    HashMapEntry* entries;
    int size;
    int capacity;
} HashMap;

// Hash function to calculate hash index
unsigned long hash(char* str) {
    unsigned long hash = 5381;
    int c;
    while ((c = *str++))
        hash = ((hash << 5) + hash) + c;
    return hash;
}

// Initialize hash map
HashMap* createHashMap(int capacity) {
    HashMap* map = (HashMap*)malloc(sizeof(HashMap));
    map->entries = (HashMapEntry*)malloc(capacity * sizeof(HashMapEntry));
    map->size = 0;
    map->capacity = capacity;
    for (int i = 0; i < capacity; i++) {
        map->entries[i].word = NULL;
        map->entries[i].count = 0;
    }
    return map;
}

// Add word to hash map
void addWord(HashMap* map, char* word) {
    unsigned long index = hash(word) % map->capacity;
    while (map->entries[index].word != NULL && strcmp(map->entries[index].word, word) != 0) {
        index = (index + 1) % map->capacity;
    }
    if (map->entries[index].word == NULL) {
        map->entries[index].word = strdup(word);
        map->size++;
    }
    map->entries[index].count++;
}

// Get word count from hash map
int getWordCount(HashMap* map, char* word) {
    unsigned long index = hash(word) % map->capacity;
    while (map->entries[index].word != NULL) {
        if (strcmp(map->entries[index].word, word) == 0) {
            return map->entries[index].count;
        }
        index = (index + 1) % map->capacity;
    }
    return 0;
}

// Tokenize sentence and add words to hash map
void processSentence(HashMap* map, char* sentence) {
    char* token = strtok(sentence, " ");
    while (token != NULL) {
        addWord(map, token);
        token = strtok(NULL, " ");
    }
}

// Function to find uncommon words
char** uncommonFromSentences(char* s1, char* s2, int* returnSize) {
    int capacity = 200;
    HashMap* map = createHashMap(capacity);

    char* s1Copy = strdup(s1);
    char* s2Copy = strdup(s2);

    // Process both sentences
    processSentence(map, s1Copy);
    processSentence(map, s2Copy);

    // Collect uncommon words
    char** result = (char**)malloc(capacity * sizeof(char*));
    *returnSize = 0;

    for (int i = 0; i < map->capacity; i++) {
        if (map->entries[i].word != NULL && map->entries[i].count == 1) {
            result[(*returnSize)++] = strdup(map->entries[i].word);
        }
    }

    free(s1Copy);
    free(s2Copy);
    free(map->entries);
    free(map);

    return result;
}